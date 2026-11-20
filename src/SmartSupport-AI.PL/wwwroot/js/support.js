(() => {
    const byId = id => document.getElementById(id);
    const desk = byId("support");
    const token = desk.querySelector("input[name=__RequestVerificationToken]").value;
    const customer = desk.dataset.customer === "true";
    const root = desk.dataset.root.replace(/\/$/, "");
    let threadId;
    let ticketId;
    let refundOrder;
    let pendingMessage;
    let lastRender = "";
    let busy = false;

    async function request(action, data, post = false) {
        const query = new URLSearchParams(data);
        const response = await fetch(root + "/" + action + (post ? "" : "?" + query), post ? {
            method: "POST", headers: { RequestVerificationToken: token }, body: query
        } : { cache: "no-store" });
        if (response.redirected) {
            location.assign(response.url);
            throw new Error("Sign in to continue.");
        }
        if (!response.ok) throw new Error(response.status === 429 ? "Too many requests. Try again shortly." :
            "The operation could not be confirmed. Check the conversation before retrying.");
        return response.json();
    }

    function showError(error) {
        byId("desk-error").textContent = error.message;
        byId("desk-error").hidden = false;
    }

    async function run(operation, activity = "") {
        if (busy) return;
        busy = true;
        byId("desk-error").hidden = true;
        byId("activity").textContent = activity;
        const buttons = [...desk.querySelectorAll("form button, #start-thread")];
        buttons.forEach(button => button.disabled = true);
        try { await operation(); }
        catch (error) { showError(error); }
        finally {
            busy = false;
            buttons.forEach(button => button.disabled = false);
            if (byId("activity").textContent === activity) byId("activity").textContent = "";
        }
    }

    function listItem(list, text, click) {
        const button = document.createElement("button");
        button.type = "button";
        button.className = "list-group-item list-group-item-action";
        button.textContent = text;
        button.addEventListener("click", () => run(click));
        list.append(button);
    }

    async function dashboard() {
        const data = await request("Dashboard");
        const threads = byId("threads");
        if (threads) {
            threads.replaceChildren();
            for (const id of data.conversations) listItem(threads, "Conversation #" + id, () => open(id));
            if (!data.conversations.length) threads.textContent = "No conversations yet.";
        }
        const tickets = byId("tickets");
        tickets.replaceChildren();
        for (const ticket of data.tickets) {
            const join = !customer && ["Queued", "Assigned"].includes(ticket.status);
            listItem(tickets, "#" + ticket.id + " · " + ticket.status + (join ? " · Join" : ""), async () => {
                if (join) await request("Ticket", { ticketId: ticket.id, action: "Join" }, true);
                await open(ticket.conversationId);
                await dashboard();
            });
        }
        if (!data.tickets.length) tickets.textContent = "No tickets yet.";
    }

    async function open(id) {
        if (threadId !== id && customer) byId("refund-form").hidden = true;
        threadId = id;
        await refreshThread();
    }

    async function refreshThread() {
        if (!threadId) return;
        const id = threadId;
        const data = await request("Thread", { id });
        if (id !== threadId) return;
        const names = { AI: "AI support", Waiting: "Waiting for an agent", Human: data.agentOnline ? "Agent joined" : "Agent disconnected", Closed: "Closed" };
        byId("thread-name").textContent = "Conversation #" + id + " · " + (names[data.status] || data.status);
        byId("send-message").hidden = data.status === "Closed" || (!customer && data.status !== "Human");
        const ticket = data.tickets.find(item => item.status === "InProgress") || data.tickets.at(-1);
        ticketId = ticket?.id;
        byId("ticket-context").textContent = ticket ? "Ticket #" + ticket.id + " · " + ticket.status +
            (ticket.summary ? "\n" + ticket.summary : "") : "";
        if (customer) {
            byId("feedback-form").hidden = !data.messages.length;
            if (data.status !== "AI") byId("refund-form").hidden = true;
        } else {
            byId("note-form").hidden = data.status !== "Human";
            byId("agent-details").hidden = !data.agentDetails;
            if (data.agentDetails) {
                const { history, notes, toolOutcomes } = data.agentDetails;
                byId("agent-detail-items").textContent = [
                    "Status history", ...history, "", "Private notes", ...notes, "", "Tool outcomes", ...toolOutcomes
                ].join("\n");
            }
        }
        const snapshot = JSON.stringify([id, data.messages]);
        if (snapshot === lastRender) return;
        lastRender = snapshot;
        const log = byId("chat-log");
        const fragment = document.createDocumentFragment();
        for (const message of data.messages) {
            const bubble = document.createElement("div");
            bubble.className = "support-bubble support-" + (message.sender === "Customer" ? "customer" : "reply");
            const label = document.createElement("strong");
            label.textContent = { AI: "SmartSupport AI", Agent: "Support agent", Customer: "Customer" }[message.sender] || "Support";
            bubble.append(label, document.createTextNode("\n" + message.text));
            fragment.append(bubble);
        }
        log.replaceChildren(fragment);
        log.scrollTop = log.scrollHeight;
    }

    function bindForm(id, handler, status) {
        byId(id)?.addEventListener("submit", event => {
            event.preventDefault();
            const form = event.currentTarget;
            run(() => handler(form, Object.fromEntries(new FormData(form))), typeof status === "function" ? status(form) : status);
        });
    }

    byId("start-thread")?.addEventListener("click", () => run(async () => {
        const created = await request("Start", {}, true);
        await open(created.id);
        await dashboard();
        byId("message").focus();
    }));

    bindForm("send-message", async (form, values) => {
        const key = JSON.stringify([threadId, values.message]);
        if (pendingMessage?.key !== key) pendingMessage = { key, id: crypto.randomUUID() };
        const result = await request("Send", { id: threadId, requestId: pendingMessage.id, ...values }, true);
        pendingMessage = null;
        form.reset();
        if (customer) {
            refundOrder = result.offer?.orderId;
            byId("refund-form").hidden = !refundOrder;
            if (refundOrder) byId("refund-summary").textContent = "Submit a refund request for order " + result.offer.orderNumber +
                " (amount " + result.offer.amount + "). The request will be reviewed.";
        }
        await refreshThread();
        await dashboard();
    }, form => /refund/i.test(form.elements.message.value) ? "Checking refund eligibility…" :
        /order|tracking/i.test(form.elements.message.value) ? "Checking your order…" :
        /human|agent/i.test(form.elements.message.value) ? "Requesting human support…" : "Sending message…");

    bindForm("refund-form", async (form, values) => {
        const result = await request("Refund", { id: threadId, orderId: refundOrder, ...values }, true);
        form.reset();
        form.hidden = true;
        byId("activity").textContent = "Refund request #" + result.refundId + " submitted for review.";
        await refreshThread();
        await dashboard();
    }, "Submitting your refund request…");

    bindForm("feedback-form", async (_, values) => {
        await request("Feedback", { id: threadId, ...values }, true);
        byId("activity").textContent = "Rating saved.";
    });

    bindForm("note-form", async (form, values) => {
        await request("Ticket", { ticketId, action: "Note", ...values }, true);
        form.reset();
        byId("activity").textContent = "Private note saved.";
    });

    byId("resolve")?.addEventListener("click", () => run(async () => {
        await request("Ticket", { ticketId, action: "Resolve" }, true);
        await refreshThread();
        await dashboard();
    }));

    async function refresh() {
        if (busy) return;
        try { await dashboard(); await refreshThread(); }
        catch (error) { showError(error); }
    }

    function connect() {
        const url = new URL(desk.dataset.hub, location.href);
        url.protocol = location.protocol === "https:" ? "wss:" : "ws:";
        const socket = new WebSocket(url);
        const separator = String.fromCharCode(30);
        let ready = false;
        const handshakeDeadline = setTimeout(() => socket.close(), 10000);
        socket.onopen = () => socket.send(JSON.stringify({ protocol: "json", version: 1 }) + separator);
        socket.onmessage = event => {
            for (const frame of event.data.split(separator).filter(Boolean)) {
                const payload = JSON.parse(frame);
                if (payload.error || payload.type === 7) { socket.close(); return; }
                if (!ready) {
                    ready = true;
                    clearTimeout(handshakeDeadline);
                    byId("connection").textContent = "Live updates connected";
                    refresh();
                }
                if (payload.type === 1 && payload.target === "Changed") refresh();
            }
        };
        const ping = setInterval(() => {
            if (ready && socket.readyState === WebSocket.OPEN) socket.send('{"type":6}' + separator);
        }, 10000);
        socket.onclose = () => {
            clearTimeout(handshakeDeadline);
            clearInterval(ping);
            byId("connection").textContent = "Live updates disconnected";
            setTimeout(connect, 5000);
        };
    }
    refresh();
    connect();
    setInterval(refresh, 6000);
})();
