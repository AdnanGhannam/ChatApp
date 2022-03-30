using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Application.EventsHandlers
{
    /*
    public class OnCommentOnStoryEventHandler
           : INotificationHandler<DomainEventNotification<CommentOnStoryEvent>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OnCommentOnStoryEventHandler> _logger;

        public OnCommentOnStoryEventHandler(AppDbContext context,
            ILogger<OnCommentOnStoryEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<CommentOnStoryEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            try
            {
                _logger.LogDebug($"Dispatching Domain Event");

                var author = _context.Stories
                    .Include(s => s.Author)
                    .FirstOrDefault(s => s.Id == domainEvent.StoryId)
                    .Author;

                if (author.Settings.NotificationOnInteract)
                {
                    var user = await _context.Users.FindAsync(domainEvent.UserId);

                    author.AddNotification(
                        new($"{ user.UserName } just published a new story",
                            domainEvent.StoryId,
                            user.UserName,
                            NotificationType.Comment));
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Error handling domain event { domainEvent.GetType() }");
                throw;
            }
        }
    }
    */
}
