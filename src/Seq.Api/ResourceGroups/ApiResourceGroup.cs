﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Seq.Api.Client;
using Seq.Api.Model;

namespace Seq.Api.ResourceGroups
{
    [DebuggerDisplay("{_name}")]
    public class ApiResourceGroup
    {
        readonly string _name;
        readonly ISeqConnection _connection;

        internal ApiResourceGroup(string name, ISeqConnection connection)
        {
            _name = name;
            _connection = connection;
        }

        protected SeqApiClient Client => _connection.Client;

        protected Task<ResourceGroup> LoadGroupAsync()
        {
            return _connection.LoadResourceGroupAsync(_name);
        }

        protected async Task<TEntity> GroupGetAsync<TEntity>(string link, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.GetAsync<TEntity>(group, link, parameters).ConfigureAwait(false);
        }

        protected async Task<string> GroupGetStringAsync(string link, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.GetStringAsync(group, link, parameters).ConfigureAwait(false);
        }

        protected async Task<List<TEntity>> GroupListAsync<TEntity>(string link, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.ListAsync<TEntity>(group, link, parameters).ConfigureAwait(false);
        }

        protected async Task GroupPostAsync<TEntity>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            await Client.PostAsync(group, link, content, parameters).ConfigureAwait(false);
        }

        protected async Task<string> GroupPostReadStringAsync<TEntity>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.PostReadStringAsync(group, link, content, parameters).ConfigureAwait(false);
        }

        protected async Task<Stream> GroupPostReadBytesAsync<TEntity>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.PostReadStreamAsync(group, link, content, parameters).ConfigureAwait(false);
        }

        protected async Task<TResponse> GroupPostAsync<TEntity, TResponse>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.PostAsync<TEntity, TResponse>(group, link, content, parameters).ConfigureAwait(false);
        }

        protected async Task GroupPutAsync<TEntity>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            await Client.PutAsync(group, link, content, parameters).ConfigureAwait(false);
        }

        protected async Task GroupDeleteAsync<TEntity>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            await Client.DeleteAsync(group, link, content, parameters).ConfigureAwait(false);
        }

        protected async Task<TResponse> GroupDeleteAsync<TEntity, TResponse>(string link, TEntity content, IDictionary<string, object> parameters = null)
        {
            var group = await LoadGroupAsync().ConfigureAwait(false);
            return await Client.DeleteAsync<TEntity, TResponse>(group, link, content, parameters).ConfigureAwait(false);
        }

        protected string GetLink<TEntity>(TEntity entity, string link, string orElse) where TEntity : ILinked
        {
            return entity.Links.ContainsKey(link) ? link : orElse;
        }

        protected async Task<TResponse> GroupCreateAsync<TEntity, TResponse>(TEntity entity,
            IDictionary<string, object> parameters = null) where TEntity : ILinked
        {
            ILinked resource;
            string link;

            if (entity.Links.ContainsKey("Create"))
            {
                resource = entity;
                link = "Create";
            }
            else
            {
                resource = await LoadGroupAsync().ConfigureAwait(false);
                link = "Items";
            }

            return await Client.PostAsync<TEntity, TResponse>(resource, link, entity, parameters).ConfigureAwait(false);
        }
    }
}
