using Android.App;
using Android.Database;
using Android.Provider;
using System.IO;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace ContactsViewer.Services;

public class ContactsService
{
    public async Task<List<ContactInfo>> GetContacts()
    {
        if (await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.ContactsRead>();

        if (await Permissions.CheckStatusAsync<Permissions.StorageRead>() != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.StorageRead>();

        return await Task.Run(async () =>
        {
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

                    string contactImagePath = contactDetailCursor.GetString(3);

                    if (contactImagePath != null)
                    {
                        try
                        {
                            await using var sourceStream = MauiApplication.Current.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(contactImagePath));
                            contact.Image = await sourceStream.ConvertToBase64Image();
                        }
                        catch (Java.IO.FileNotFoundException)
                        {

                        }
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

#if DEBUG
                    if (contacts.Count == 100)
                        return contacts;
#endif

                } while (contactDetailCursor.MoveToNext());
            };

            return contacts;
        });
    }
}