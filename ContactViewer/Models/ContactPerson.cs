namespace ContactViewer.Models;

public class ContactPerson
{
    public int Id { get; set; }

    public string DisplayName { get; set; }

    public List<ContactNumber> Numbers { get; set; } = new();

    public string ImagePath { get; set; }
}