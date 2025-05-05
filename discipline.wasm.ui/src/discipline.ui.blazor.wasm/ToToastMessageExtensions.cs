using BlazorBootstrap;

namespace discipline.ui.blazor.wasm;

internal static class ToToastMessageExtensions
{
    internal static ToastMessage ToDangerToastMessage(this string value)
        => new ToastMessage()
        {
            Type = ToastType.Danger,
            Message = value
        };
}