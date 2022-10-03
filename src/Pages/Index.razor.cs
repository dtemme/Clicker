using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Clicker.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Clicker.Pages
{
    public partial class Index
    {
        private const string CounterStatesStorageKey = "CounterStates";
        private const string DarkModeStorageKey = "DarkMode";

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public IList<CounterInfo> Counters { get; set; } = new List<CounterInfo>();
        public bool DarkMode { get; private set; }

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
            DarkMode = await LocalStorage.GetItemAsync<bool?>(DarkModeStorageKey).ConfigureAwait(false) ?? false;
            await SetDarkModeAsync(DarkMode).ConfigureAwait(false);
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

        public async Task ToggleDarkMode()
        {
            DarkMode = !DarkMode;
            await SetDarkModeAsync(DarkMode).ConfigureAwait(false);
            await LocalStorage.SetItemAsync(DarkModeStorageKey, DarkMode).ConfigureAwait(false);
            StateHasChanged();
        }

        private async Task SetDarkModeAsync(bool darkMode)
            => await JsRuntime.InvokeVoidAsync("setDarkMode", darkMode).ConfigureAwait(false);
    }
}
