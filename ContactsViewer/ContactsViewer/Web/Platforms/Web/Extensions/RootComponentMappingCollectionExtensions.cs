using Microsoft.AspNetCore.Components.Web;

#if BlazorWebAssembly
namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting;
#else
using Microsoft.AspNetCore.Components.Server;
#endif

public static class RootComponentMappingCollectionExtensions
{
    public static void RegisterCustomElements(
#if BlazorWebAssembly
        this RootComponentMappingCollection rootComponentMappings
#else
        this CircuitRootComponentOptions rootComponentMappings
#endif
        )
    {
        rootComponentMappings.RegisterAsCustomElement<ContactsViewer.App.Pages.Index>("index-page"); // see <index-page> tag in app.component.ts
    }
}

