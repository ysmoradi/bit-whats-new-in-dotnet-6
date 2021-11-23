namespace ContactViewer.Pages;

public partial class Index
{
    [Inject] public ContactsService ContactsService { get; set; }
    public List<ContactPerson> ContactPeople { get; set; }

    protected async override Task OnInitializedAsync()
    {
        ContactPeople = await ContactsService.GetContacts();

        await base.OnInitializedAsync();
    }
}
