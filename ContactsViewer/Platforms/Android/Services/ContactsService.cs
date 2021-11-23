using Android.App;
using Android.Database;
using Android.Provider;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace ContactsViewer.Services;

public class ContactsService
{
    public async Task<List<ContactInfo>> GetContacts()
    {
        if (await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.ContactsRead>();

        using ICursor contactDetailCursor = MauiApplication.Current.ContentResolver.Query(
            ContactsContract.Contacts.ContentUri,
            new[]
            {
                        ContactsContract.Contacts.InterfaceConsts.Id,
                        ContactsContract.Contacts.InterfaceConsts.DisplayName,
                        ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber,
                        ContactsContract.Contacts.InterfaceConsts.PhotoUri
            },
            null,
            null,
            ContactsContract.Contacts.InterfaceConsts.DisplayName
        );

        List<ContactInfo> contacts = new();

        if (contactDetailCursor.MoveToFirst())
        {
            do
            {
                if (contactDetailCursor.GetShort(2) != 1)
                    continue;

                ContactInfo contact = new()
                {
                    Id = contactDetailCursor.GetInt(0),
                    DisplayName = contactDetailCursor.GetString(1)
                };

                string imagePath = contactDetailCursor.GetString(3);

                if (imagePath != null)
                {
                    contact.ImagePath = imagePath;
                }

                using ICursor numbers = MauiApplication.Current.ContentResolver.Query(Phone.ContentUri, new string[] { Phone.Number, Phone.InterfaceConsts.Type }, $"{Phone.InterfaceConsts.ContactId} = {contact.Id}", null, null);

                while (numbers.MoveToNext())
                {
                    string number = numbers.GetString(0);
                    int type = numbers.GetInt(1);
                    bool isMobile = type == 2;

                    contact.Numbers.Add(new()
                    {
                        ContactId = contact.Id,
                        Number = number,
                        Type = isMobile ? "📱" : "🏡"
                    });

                    if (isMobile)
                    {
                        contact.Numbers.Add(new()
                        {
                            ContactId = contact.Id,
                            Number = number,
                            Type = "📷"
                        });
                    }
                }

                contacts.Add(contact);

            } while (contactDetailCursor.MoveToNext());
        };

        return contacts;
    }
}