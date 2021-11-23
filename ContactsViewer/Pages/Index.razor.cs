namespace ContactsViewer.Pages;

public partial class Index
{
    [Inject] public ContactsService ContactsService { get; set; }
    public List<ContactInfo> Contacts { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Contacts = await ContactsService.GetContacts();

        await base.OnInitializedAsync();
    }
}
