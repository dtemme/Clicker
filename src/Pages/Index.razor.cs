using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Clicker.Shared;
using Microsoft.AspNetCore.Components;

namespace Clicker.Pages
{
    public partial class Index
    {
        private const string CounterStatesStorageKey = "CounterStates";

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }
        public IList<CounterInfo> Counters { get; set; } = new List<CounterInfo>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            await LoadStateAsync().ConfigureAwait(false);
        }

        private async Task SaveStateAsync()
        {
            await LocalStorage.SetItemAsync(CounterStatesStorageKey, Counters).ConfigureAwait(false);
        }

        private async Task LoadStateAsync()
        {
            var counters = await LocalStorage.GetItemAsync<IList<CounterInfo>>(CounterStatesStorageKey).ConfigureAwait(false);
            if (counters != null)
                Counters = counters;
        }

        private async Task AddCounterAsync()
        {
            Counters.Add(new CounterInfo { Name = "Neu" });
            await SaveStateAsync().ConfigureAwait(false);
        }

        private async Task RemoveCounterAsync(CounterInfo info)
        {
            Counters.Remove(info);
            await SaveStateAsync().ConfigureAwait(false);
        }
    }
}
