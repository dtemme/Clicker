using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Clicker.Shared
{
    public partial class Counter
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public CounterInfo CounterInfo { get; set; }

        [Parameter]
        public EventCallback OnSaveState { get; set; }

        [Parameter]
        public EventCallback<CounterInfo> OnRemoveCounter { get; set; }

        private async Task IncreaseValueAsync()
        {
            CounterInfo.Value++;
            await OnSaveState.InvokeAsync().ConfigureAwait(false);
        }

        private async Task DecreaseValueAsync()
        {
            if (CounterInfo.Value > 0)
            {
                CounterInfo.Value--;
                await OnSaveState.InvokeAsync().ConfigureAwait(false);
            }
        }

        private async Task RenameCounterAsync()
        {
            var newName = await JSRuntime.InvokeAsync<string>("prompt", "Name", CounterInfo.Name).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                CounterInfo.Name = newName;
                await OnSaveState.InvokeAsync().ConfigureAwait(false);
            }
        }

        private async Task RemoveCounterAsync()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Wirklich löschen?").ConfigureAwait(false))
                await OnRemoveCounter.InvokeAsync(CounterInfo).ConfigureAwait(false);
        }
    }
}
