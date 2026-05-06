// Re-export of the interface defined in Acme.Sistemas.Core.Messaging.
// The interface lives in Core so the Services project (which doesn't
// reference Infrastructure) can depend on it.
global using IEmailQueueService = Acme.Sistemas.Core.Messaging.IEmailQueueService;
global using EmailMessage = Acme.Sistemas.Core.Messaging.EmailMessage;
global using EmailAttachment = Acme.Sistemas.Core.Messaging.EmailAttachment;
