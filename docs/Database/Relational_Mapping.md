# Mapping

## Mapping rules applied

```mermaid
flowchart TD
    A["Strong entity"] --> A1["Table"]
    A1 --> A2["Simple attributes become columns"]
    A1 --> A3["Key becomes PK"]

    B["Generalization<br/>User → Customer / SupportAgent / Administrator"] --> B1["One table per subtype"]
    B1 --> B2["Own PK"]
    B1 --> B3["user_id = FK + UNIQUE"]
    B3 --> B4["Link 1:0..1"]

    C["1:N relationship"] --> C1["FK on the N side"]
    C1 --> C2["Optional participation (0..1) → nullable FK"]

    D["M:N relationship with attributes<br/>(retrieves)"] --> D1["Junction table"]
    D1 --> D2["Composite PK made of both FKs"]
    D1 --> D3["Carries relationship attributes"]

    E["Multi-valued or composite attributes"] --> E1["None exist"]
    E1 --> E2["Nothing extra is needed"]
```

## Relational schema

**PK** is the primary key, **FK** is a foreign key, **UK** is a unique key, and ◆ marks FKs that the diagrams don’t draw as ovals.

### Users and conversations

```mermaid
flowchart TD
    User["User<br/>PK user_id<br/>full_name, email UK, password_hash<br/>phone, role, status, created_at"]
    Customer["Customer<br/>PK customer_id<br/>user_id FK→User, UK<br/>preferred_language, registered_at"]
    SupportAgent["SupportAgent<br/>PK agent_id<br/>user_id FK→User, UK<br/>department, skill_level, availability"]
    Administrator["Administrator<br/>PK admin_id<br/>user_id FK→User, UK<br/>access_level"]

    Conversation["Conversation<br/>PK conversation_id<br/>customer_id FK→Customer<br/>config_id FK→AIConfiguration<br/>channel, status, resolution_type<br/>summary, started_at, ended_at"]
    Message["Message<br/>PK message_id<br/>conversation_id FK→Conversation<br/>sequence_no, sender_type, content, sent_at<br/>customer_id ◆ FK→Customer, NULL<br/>agent_id ◆ FK→SupportAgent, NULL<br/>UK(conversation_id, sequence_no)"]
    Attachment["Attachment<br/>PK attachment_id<br/>message_id FK→Message, NULL<br/>ticket_id FK→SupportTicket, NULL<br/>uploaded_by FK→User<br/>file_name, file_type, file_size, uploaded_at<br/>CHECK(message_id or ticket_id is not null)"]
    CustomerFeedback["CustomerFeedback<br/>PK feedback_id<br/>conversation_id FK→Conversation<br/>response_id FK→AIResponse, NULL<br/>customer_id ◆ FK→Customer<br/>rating, is_helpful, comment<br/>CHECK(rating between 1 and 5)"]
    ActionLog["ActionLog<br/>PK log_id<br/>user_id FK→User, NULL<br/>conversation_id FK→Conversation, NULL<br/>ticket_id FK→SupportTicket, NULL<br/>action_type, details, created_at"]

    User -->|"user_id FK + UK"| Customer
    User -->|"user_id FK + UK"| SupportAgent
    User -->|"user_id FK + UK"| Administrator
    Customer -->|"customer_id FK"| Conversation
    Conversation -->|"conversation_id FK"| Message
    Message -->|"message_id FK, NULL"| Attachment
    Conversation -->|"conversation_id FK"| CustomerFeedback
    Message -->|"sender customer_id"| Customer
    Message -->|"sender agent_id"| SupportAgent
    User -->|"uploaded_by FK"| Attachment
    User -->|"user_id FK, NULL"| ActionLog
    Conversation -->|"conversation_id FK, NULL"| ActionLog
    Attachment -->|"ticket_id FK, NULL"| SupportTicket
    SupportTicket -->|"ticket_id FK, NULL"| ActionLog
```

### AI, knowledge base and configuration

```mermaid
flowchart TD
    AIConfiguration["AIConfiguration<br/>PK config_id<br/>created_by FK→Administrator<br/>version_number UK, model_name<br/>min_confidence, is_active, created_at"]
    Intent["Intent<br/>PK intent_id<br/>name UK, description"]
    AIResponse["AIResponse<br/>PK response_id<br/>message_id FK→Message, UK<br/>trigger_msg_id FK→Message<br/>intent_id FK→Intent<br/>config_id FK→AIConfiguration<br/>confidence, escalated, created_at"]
    ToolExecution["ToolExecution<br/>PK execution_id<br/>response_id FK→AIResponse<br/>tool_name, input_params, output_result<br/>status, executed_at"]
    ArticleCategory["ArticleCategory<br/>PK category_id<br/>name UK, description"]
    KnowledgeArticle["KnowledgeArticle<br/>PK article_id<br/>category_id FK→ArticleCategory<br/>created_by FK→Administrator<br/>title, content, status, updated_at"]
    ResponseArticle["ResponseArticle (junction for retrieves)<br/>PK (response_id, article_id)<br/>response_id FK→AIResponse<br/>article_id FK→KnowledgeArticle<br/>relevance_score, rank_position, was_used"]
    SystemPolicy["SystemPolicy<br/>PK policy_id<br/>updated_by FK→Administrator<br/>policy_name UK, policy_value, updated_at"]
    PerformanceMetric["PerformanceMetric<br/>PK metric_id<br/>agent_id FK→SupportAgent, NULL<br/>config_id FK→AIConfiguration, NULL<br/>metric_type, metric_value<br/>period_start, period_end"]

    Administrator -->|"created_by FK"| AIConfiguration
    AIConfiguration -->|"config_id FK"| Conversation
    AIConfiguration -->|"config_id FK"| AIResponse
    Message -->|"message_id FK, UK"| AIResponse
    Message -->|"trigger_msg_id FK"| AIResponse
    Intent -->|"intent_id FK"| AIResponse
    AIResponse -->|"response_id FK"| ToolExecution
    ArticleCategory -->|"category_id FK"| KnowledgeArticle
    Administrator -->|"created_by FK"| KnowledgeArticle
    AIResponse -->|"response_id FK"| ResponseArticle
    KnowledgeArticle -->|"article_id FK"| ResponseArticle
    Administrator -->|"updated_by FK"| SystemPolicy
    SupportAgent -->|"agent_id FK, NULL"| PerformanceMetric
    AIConfiguration -->|"config_id FK, NULL"| PerformanceMetric
```

### Tickets

```mermaid
flowchart TD
    SupportTicket["SupportTicket<br/>PK ticket_id<br/>conversation_id FK→Conversation<br/>customer_id FK→Customer<br/>priority, status, escalation_reason<br/>conv_summary, created_at"]
    TicketAssignment["TicketAssignment<br/>PK assignment_id<br/>ticket_id FK→SupportTicket<br/>agent_id FK→SupportAgent<br/>assigned_at, is_current"]
    TicketStatusHistory["TicketStatusHistory<br/>PK history_id<br/>ticket_id FK→SupportTicket<br/>changed_by FK→User<br/>old_status, new_status, changed_at"]
    InternalNote["InternalNote<br/>PK note_id<br/>ticket_id FK→SupportTicket<br/>agent_id FK→SupportAgent<br/>content, created_at"]

    Conversation -->|"conversation_id FK"| SupportTicket
    Customer -->|"customer_id FK"| SupportTicket
    SupportTicket -->|"ticket_id FK"| TicketAssignment
    SupportAgent -->|"agent_id FK"| TicketAssignment
    SupportTicket -->|"ticket_id FK"| TicketStatusHistory
    User -->|"changed_by FK"| TicketStatusHistory
    SupportTicket -->|"ticket_id FK"| InternalNote
    SupportAgent -->|"agent_id FK"| InternalNote
```

### Orders and refunds

```mermaid
flowchart TD
    Order["Order<br/>PK order_id<br/>customer_id FK→Customer<br/>order_number UK<br/>order_status, total_amount, placed_at"]
    OrderStatusCheck["OrderStatusCheck<br/>PK check_id<br/>order_id FK→Order<br/>conversation_id FK→Conversation<br/>execution_id ◆ FK→ToolExecution, NULL<br/>status_returned, checked_at"]
    RefundRequest["RefundRequest<br/>PK refund_id<br/>order_id FK→Order<br/>conversation_id FK→Conversation<br/>execution_id ◆ FK→ToolExecution, NULL<br/>amount, reason, eligibility<br/>status, initiated_at"]
    RefundStatusHistory["RefundStatusHistory<br/>PK history_id<br/>refund_id FK→RefundRequest<br/>old_status, new_status, changed_at"]

    Customer -->|"customer_id FK"| Order
    Order -->|"order_id FK"| OrderStatusCheck
    Conversation -->|"conversation_id FK"| OrderStatusCheck
    ToolExecution -->|"execution_id FK, NULL"| OrderStatusCheck
    Order -->|"order_id FK"| RefundRequest
    Conversation -->|"conversation_id FK"| RefundRequest
    ToolExecution -->|"execution_id FK, NULL"| RefundRequest
    RefundRequest -->|"refund_id FK"| RefundStatusHistory
```

## Relationship-to-FK mapping

```mermaid
flowchart TD
    R1["is a (1)"] --> M1["user_id UNIQUE FK in each of the 3 subtype tables"]
    R2["starts, governs, contains (2–4)"] --> M2["Conversation.customer_id<br/>Conversation.config_id<br/>Message.conversation_id"]
    R3["sends (5, 6)"] --> M3["Message.customer_id and Message.agent_id, both nullable;<br/>if both are NULL the AI sent it"]
    R4["recorded as (7)"] --> M4["AIResponse.message_id, UNIQUE (1:0..1)"]
    R5["triggers, classifies, produces (8–10)"] --> M5["AIResponse.trigger_msg_id, intent_id, config_id"]
    R6["retrieves (11), M:N"] --> M6["junction table ResponseArticle"]
    R7["groups, manages, invokes (12–14)"] --> M7["category_id, created_by, ToolExecution.response_id"]
    R8["carries, includes, uploads (15–17)"] --> M8["Attachment.message_id, ticket_id, uploaded_by"]
    R9["rated in, gives, evaluated by (18–20)"] --> M9["CustomerFeedback.conversation_id, customer_id, response_id"]
    R10["escalates to, raises (21, 22)"] --> M10["SupportTicket.conversation_id, customer_id"]
    R11["assigned via, handles (23, 24)"] --> M11["TicketAssignment.ticket_id, agent_id"]
    R12["tracked in, changes, has, writes (25–28)"] --> M12["FKs on TicketStatusHistory and InternalNote"]
    R13["places, checked via, involves, performed by (29–32)"] --> M13["FKs on Order and OrderStatusCheck"]
    R14["refunded by, initiates, executed by, tracked in (33–36)"] --> M14["FKs on RefundRequest and RefundStatusHistory"]
    R15["creates, maintains (37, 38)"] --> M15["AIConfiguration.created_by, SystemPolicy.updated_by"]
    R16["measured by (39, 40)"] --> M16["PerformanceMetric.agent_id, config_id, both nullable"]
    R17["performs, logged in (41–43)"] --> M17["ActionLog.user_id, conversation_id, ticket_id, all nullable"]
```

## Points to watch in the source document

- **Counts don’t match.** The overview says 25 entities and 42 relationships, but the dictionary has 26 tables (25 entities plus the `retrieves` junction) and section 6 lists 43 relationships.
- **Reserved words.** `User` and `Order` are reserved words in SQL Server, so write them as `[User]` and `[Order]`, or name them `Users` and `Orders`.
- **Circular FK path.** `Conversation` and `SupportTicket` reference each other only indirectly, but `Attachment` points to both `Message` and `SupportTicket`. Create tables in this order: User → subtypes → AIConfiguration → Conversation → Message → AIResponse → SupportTicket → Attachment.
- **Cascade conflicts.** `AIResponse` has two FKs to `Message`. SQL Server rejects multiple cascade paths, so use `ON DELETE NO ACTION` on `trigger_msg_id`.
