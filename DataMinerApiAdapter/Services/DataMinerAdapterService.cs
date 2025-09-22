using DataMinerApiAdapter.Hubs;
using DataMinerApiAdapter.Models;
using Microsoft.AspNet.SignalR;
using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Net.SubscriptionFilters;
using System;

namespace DataMinerApiAdapter.Services
{
    public class DataMinerAdapterService : IDisposable
    {
        public static DataMinerAdapterService Instance { get; } = new DataMinerAdapterService();

        private readonly Connection _connection;

        /// <summary>
        /// This shows how a connection can be created to DataMiner. This is only an example, if implemented, don't forget to:
        /// - Store your credentials in a safely
        /// - Subscribe & Unsubscribe when needed. Try to keep your filters as specific as possible to avoid receiving too many unwanted events
        /// - Add a safety mechanism that would re-create a connection if it would become invalid (OnAbnormalClose event e.g.)
        /// </summary>
        private DataMinerAdapterService()
        {
            _connection = ConnectionSettings.GetConnection("localhost");
            _connection.Authenticate("service_user", "52779571-1d11-4be0-a36c-8d0440403e82"); 
            
            // Subscribe to DOM instance events for DOM module 'my_dom_module'
            var setId = Guid.NewGuid().ToString(); // Can be something random
            _connection.AddSubscription(setId, new ModuleEventSubscriptionFilter<DomInstancesChangedEventMessage>("my_dom_module"));
            _connection.OnNewMessage += ConnectionOnOnNewMessage;
            _connection.Subscribe();
        }

        private void ConnectionOnOnNewMessage(object sender, NewMessageEventArgs e)
        {
            if (e.Message is DomInstancesChangedEventMessage domEvent)
            {
                var webSocketEvent = new JobsUpdatedEvent
                {
                    ModuleId = domEvent.ModuleId,
                    AmountOfCreated = domEvent.Created.Count,
                    AmountOfUpdated = domEvent.Updated.Count,
                    AmountOfDeleted = domEvent.Deleted.Count
                };

                // Send to all clients via SignalR
                var context = GlobalHost.ConnectionManager.GetHubContext<JobHub>();
                context.Clients.All.jobsUpdated(webSocketEvent);
            }
        }

        public DMSMessage[] HandleMessages(DMSMessage[] request)
        {
            return _connection.HandleMessages(request);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}