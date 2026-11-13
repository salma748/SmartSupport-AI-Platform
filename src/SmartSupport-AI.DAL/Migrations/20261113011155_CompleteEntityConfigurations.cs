using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSupport_AI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompleteEntityConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PolicyName",
                table: "SystemPolicies",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Intents",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ArticleCategories",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "AIConfigurations",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_ResponseId",
                table: "ToolExecutions",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatusHistories_ChangedBy",
                table: "TicketStatusHistories",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatusHistories_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_AgentId",
                table: "TicketAssignments",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPolicies_PolicyName",
                table: "SystemPolicies",
                column: "PolicyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemPolicies_UpdatedBy",
                table: "SystemPolicies",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_ConversationId",
                table: "SupportTickets",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CustomerId",
                table: "SupportTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportAgents_UserId",
                table: "SupportAgents",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResponseArticles_ArticleId",
                table: "ResponseArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundStatusHistories_RefundId",
                table: "RefundStatusHistories",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundRequests_ConversationId",
                table: "RefundRequests",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundRequests_ExecutionId",
                table: "RefundRequests",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundRequests_OrderId",
                table: "RefundRequests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceMetrics_AgentId",
                table: "PerformanceMetrics",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceMetrics_ConfigId",
                table: "PerformanceMetrics",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusChecks_ConversationId",
                table: "OrderStatusChecks",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusChecks_ExecutionId",
                table: "OrderStatusChecks",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusChecks_OrderId",
                table: "OrderStatusChecks",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AgentId",
                table: "Messages",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId_SequenceNo",
                table: "Messages",
                columns: new[] { "ConversationId", "SequenceNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CustomerId",
                table: "Messages",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_CategoryId",
                table: "KnowledgeArticles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_CreatedBy",
                table: "KnowledgeArticles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalNotes_AgentId",
                table: "InternalNotes",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalNotes_TicketId",
                table: "InternalNotes",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Intents_Name",
                table: "Intents",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedbacks_ConversationId",
                table: "CustomerFeedbacks",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedbacks_CustomerId",
                table: "CustomerFeedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedbacks_ResponseId",
                table: "CustomerFeedbacks",
                column: "ResponseId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CustomerFeedback_Rating",
                table: "CustomerFeedbacks",
                sql: "[Rating] >= 1 AND [Rating] <= 5");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ConfigId",
                table: "Conversations",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CustomerId",
                table: "Conversations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_MessageId",
                table: "Attachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TicketId",
                table: "Attachments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadedBy",
                table: "Attachments",
                column: "UploadedBy");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Attachment_MessageOrTicket",
                table: "Attachments",
                sql: "[MessageId] IS NOT NULL OR [TicketId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategories_Name",
                table: "ArticleCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_ConfigId",
                table: "AIResponses",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_IntentId",
                table: "AIResponses",
                column: "IntentId");

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_MessageId",
                table: "AIResponses",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_TriggerMsgId",
                table: "AIResponses",
                column: "TriggerMsgId");

            migrationBuilder.CreateIndex(
                name: "IX_AIConfigurations_CreatedBy",
                table: "AIConfigurations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AIConfigurations_VersionNumber",
                table: "AIConfigurations",
                column: "VersionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_UserId",
                table: "Administrators",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_ConversationId",
                table: "ActionLogs",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_TicketId",
                table: "ActionLogs",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_UserId",
                table: "ActionLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLogs_Conversations_ConversationId",
                table: "ActionLogs",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLogs_SupportTickets_TicketId",
                table: "ActionLogs",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLogs_Users_UserId",
                table: "ActionLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Users_UserId",
                table: "Administrators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIConfigurations_Administrators_CreatedBy",
                table: "AIConfigurations",
                column: "CreatedBy",
                principalTable: "Administrators",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIResponses_AIConfigurations_ConfigId",
                table: "AIResponses",
                column: "ConfigId",
                principalTable: "AIConfigurations",
                principalColumn: "ConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIResponses_Intents_IntentId",
                table: "AIResponses",
                column: "IntentId",
                principalTable: "Intents",
                principalColumn: "IntentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIResponses_Messages_MessageId",
                table: "AIResponses",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AIResponses_Messages_TriggerMsgId",
                table: "AIResponses",
                column: "TriggerMsgId",
                principalTable: "Messages",
                principalColumn: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_SupportTickets_TicketId",
                table: "Attachments",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_UploadedBy",
                table: "Attachments",
                column: "UploadedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AIConfigurations_ConfigId",
                table: "Conversations",
                column: "ConfigId",
                principalTable: "AIConfigurations",
                principalColumn: "ConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Customers_CustomerId",
                table: "Conversations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedbacks_AIResponses_ResponseId",
                table: "CustomerFeedbacks",
                column: "ResponseId",
                principalTable: "AIResponses",
                principalColumn: "ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedbacks_Conversations_ConversationId",
                table: "CustomerFeedbacks",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedbacks_Customers_CustomerId",
                table: "CustomerFeedbacks",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalNotes_SupportAgents_AgentId",
                table: "InternalNotes",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalNotes_SupportTickets_TicketId",
                table: "InternalNotes",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeArticles_Administrators_CreatedBy",
                table: "KnowledgeArticles",
                column: "CreatedBy",
                principalTable: "Administrators",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeArticles_ArticleCategories_CategoryId",
                table: "KnowledgeArticles",
                column: "CategoryId",
                principalTable: "ArticleCategories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Customers_CustomerId",
                table: "Messages",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_SupportAgents_AgentId",
                table: "Messages",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatusChecks_Conversations_ConversationId",
                table: "OrderStatusChecks",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatusChecks_Orders_OrderId",
                table: "OrderStatusChecks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatusChecks_ToolExecutions_ExecutionId",
                table: "OrderStatusChecks",
                column: "ExecutionId",
                principalTable: "ToolExecutions",
                principalColumn: "ExecutionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceMetrics_AIConfigurations_ConfigId",
                table: "PerformanceMetrics",
                column: "ConfigId",
                principalTable: "AIConfigurations",
                principalColumn: "ConfigId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceMetrics_SupportAgents_AgentId",
                table: "PerformanceMetrics",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_Conversations_ConversationId",
                table: "RefundRequests",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_Orders_OrderId",
                table: "RefundRequests",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_ToolExecutions_ExecutionId",
                table: "RefundRequests",
                column: "ExecutionId",
                principalTable: "ToolExecutions",
                principalColumn: "ExecutionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundStatusHistories_RefundRequests_RefundId",
                table: "RefundStatusHistories",
                column: "RefundId",
                principalTable: "RefundRequests",
                principalColumn: "RefundId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponseArticles_AIResponses_ResponseId",
                table: "ResponseArticles",
                column: "ResponseId",
                principalTable: "AIResponses",
                principalColumn: "ResponseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponseArticles_KnowledgeArticles_ArticleId",
                table: "ResponseArticles",
                column: "ArticleId",
                principalTable: "KnowledgeArticles",
                principalColumn: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportAgents_Users_UserId",
                table: "SupportAgents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Conversations_ConversationId",
                table: "SupportTickets",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Customers_CustomerId",
                table: "SupportTickets",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemPolicies_Administrators_UpdatedBy",
                table: "SystemPolicies",
                column: "UpdatedBy",
                principalTable: "Administrators",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_SupportAgents_AgentId",
                table: "TicketAssignments",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_SupportTickets_TicketId",
                table: "TicketAssignments",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketStatusHistories_SupportTickets_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketStatusHistories_Users_ChangedBy",
                table: "TicketStatusHistories",
                column: "ChangedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolExecutions_AIResponses_ResponseId",
                table: "ToolExecutions",
                column: "ResponseId",
                principalTable: "AIResponses",
                principalColumn: "ResponseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLogs_Conversations_ConversationId",
                table: "ActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionLogs_SupportTickets_TicketId",
                table: "ActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionLogs_Users_UserId",
                table: "ActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Users_UserId",
                table: "Administrators");

            migrationBuilder.DropForeignKey(
                name: "FK_AIConfigurations_Administrators_CreatedBy",
                table: "AIConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_AIResponses_AIConfigurations_ConfigId",
                table: "AIResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_AIResponses_Intents_IntentId",
                table: "AIResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_AIResponses_Messages_MessageId",
                table: "AIResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_AIResponses_Messages_TriggerMsgId",
                table: "AIResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_SupportTickets_TicketId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_UploadedBy",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AIConfigurations_ConfigId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Customers_CustomerId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedbacks_AIResponses_ResponseId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedbacks_Conversations_ConversationId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedbacks_Customers_CustomerId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_UserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalNotes_SupportAgents_AgentId",
                table: "InternalNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalNotes_SupportTickets_TicketId",
                table: "InternalNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeArticles_Administrators_CreatedBy",
                table: "KnowledgeArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeArticles_ArticleCategories_CategoryId",
                table: "KnowledgeArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Customers_CustomerId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_SupportAgents_AgentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusChecks_Conversations_ConversationId",
                table: "OrderStatusChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusChecks_Orders_OrderId",
                table: "OrderStatusChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusChecks_ToolExecutions_ExecutionId",
                table: "OrderStatusChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceMetrics_AIConfigurations_ConfigId",
                table: "PerformanceMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceMetrics_SupportAgents_AgentId",
                table: "PerformanceMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_Conversations_ConversationId",
                table: "RefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_Orders_OrderId",
                table: "RefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_ToolExecutions_ExecutionId",
                table: "RefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundStatusHistories_RefundRequests_RefundId",
                table: "RefundStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponseArticles_AIResponses_ResponseId",
                table: "ResponseArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponseArticles_KnowledgeArticles_ArticleId",
                table: "ResponseArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportAgents_Users_UserId",
                table: "SupportAgents");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Conversations_ConversationId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Customers_CustomerId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemPolicies_Administrators_UpdatedBy",
                table: "SystemPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_SupportAgents_AgentId",
                table: "TicketAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_SupportTickets_TicketId",
                table: "TicketAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketStatusHistories_SupportTickets_TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketStatusHistories_Users_ChangedBy",
                table: "TicketStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolExecutions_AIResponses_ResponseId",
                table: "ToolExecutions");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ToolExecutions_ResponseId",
                table: "ToolExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TicketStatusHistories_ChangedBy",
                table: "TicketStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketStatusHistories_TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketAssignments_AgentId",
                table: "TicketAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SystemPolicies_PolicyName",
                table: "SystemPolicies");

            migrationBuilder.DropIndex(
                name: "IX_SystemPolicies_UpdatedBy",
                table: "SystemPolicies");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_ConversationId",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_CustomerId",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SupportAgents_UserId",
                table: "SupportAgents");

            migrationBuilder.DropIndex(
                name: "IX_ResponseArticles_ArticleId",
                table: "ResponseArticles");

            migrationBuilder.DropIndex(
                name: "IX_RefundStatusHistories_RefundId",
                table: "RefundStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_RefundRequests_ConversationId",
                table: "RefundRequests");

            migrationBuilder.DropIndex(
                name: "IX_RefundRequests_ExecutionId",
                table: "RefundRequests");

            migrationBuilder.DropIndex(
                name: "IX_RefundRequests_OrderId",
                table: "RefundRequests");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceMetrics_AgentId",
                table: "PerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceMetrics_ConfigId",
                table: "PerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusChecks_ConversationId",
                table: "OrderStatusChecks");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusChecks_ExecutionId",
                table: "OrderStatusChecks");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusChecks_OrderId",
                table: "OrderStatusChecks");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AgentId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ConversationId_SequenceNo",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_CustomerId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeArticles_CategoryId",
                table: "KnowledgeArticles");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeArticles_CreatedBy",
                table: "KnowledgeArticles");

            migrationBuilder.DropIndex(
                name: "IX_InternalNotes_AgentId",
                table: "InternalNotes");

            migrationBuilder.DropIndex(
                name: "IX_InternalNotes_TicketId",
                table: "InternalNotes");

            migrationBuilder.DropIndex(
                name: "IX_Intents_Name",
                table: "Intents");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UserId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedbacks_ConversationId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedbacks_CustomerId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedbacks_ResponseId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CustomerFeedback_Rating",
                table: "CustomerFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_ConfigId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_CustomerId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_MessageId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_TicketId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_UploadedBy",
                table: "Attachments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Attachment_MessageOrTicket",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_ArticleCategories_Name",
                table: "ArticleCategories");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_ConfigId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_IntentId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_MessageId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_TriggerMsgId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIConfigurations_CreatedBy",
                table: "AIConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_AIConfigurations_VersionNumber",
                table: "AIConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_Administrators_UserId",
                table: "Administrators");

            migrationBuilder.DropIndex(
                name: "IX_ActionLogs_ConversationId",
                table: "ActionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActionLogs_TicketId",
                table: "ActionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActionLogs_UserId",
                table: "ActionLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "PolicyName",
                table: "SystemPolicies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Intents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ArticleCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "AIConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
