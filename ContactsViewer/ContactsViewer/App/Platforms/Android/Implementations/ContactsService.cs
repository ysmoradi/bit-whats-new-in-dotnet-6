using Android.Database;
using Android.Provider;
using System.Collections.Concurrent;

namespace ContactsViewer.App.Platforms.Android.Implementations;

public class ContactsService : IContactsService
{
    public static ConcurrentDictionary<string, string> ContactPhotos = new();

    public async Task<List<ContactInfo>> GetContacts()
    {
        if (await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.ContactsRead>();

        return await Task.Run(async () =>
        {
            using ICursor contactDetailCursor = MauiApplication.Current.ContentResolver.Query(
            ContactsContract.Contacts.ContentUri,
            new[]
            {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber,
                ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri
            },
            null,
            null,
            ContactsContract.Contacts.InterfaceConsts.DisplayName);

            List<ContactInfo> contacts = new();

            if (contactDetailCursor.MoveToFirst())
            {
                do
                {
                    if (contactDetailCursor.GetShort(2) != 1)
                        continue;

                    ContactInfo contact = new()
                    {
                        Id = contactDetailCursor.GetString(0),
                        DisplayName = contactDetailCursor.GetString(1)
                    };

                    string photoUrl = contactDetailCursor.GetString(3);

                    if (photoUrl is not null)
                    {
                        ContactPhotos.TryAdd(contact.Id, photoUrl);
                        contact.Image = $"{contact.Id}.contact"; // ContactsViewFileProvider
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