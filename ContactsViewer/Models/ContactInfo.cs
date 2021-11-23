namespace ContactsViewer.Models;

public class ContactInfo
{
    public int Id { get; set; }

    public string DisplayName { get; set; }

    public List<ContactPhoneNumber> Numbers { get; set; } = new();

    public string ImagePath { get; set; }
}