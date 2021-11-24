namespace ContactsViewer.Pages;

public partial class Index
{
    [Inject] public ContactsService ContactsService { get; set; }
    public List<ContactInfo> Contacts { get; set; }

    public void Reload()
    {
        StateHasChanged(); // to perform hot reload in blazor hybrid. It's not required in blazor wasm, server modes.
    }

    protected async override Task OnInitializedAsync()
    {
#if DEBUG
        await Task.Delay(TimeSpan.FromSeconds(2));
#endif

        Contacts = await ContactsService.GetContacts();

        await base.OnInitializedAsync();
    }
}
