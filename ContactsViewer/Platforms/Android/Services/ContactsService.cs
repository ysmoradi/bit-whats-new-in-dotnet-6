﻿using Android.Database;
using Android.Provider;

namespace ContactsViewer.Services;

public class ContactsService
{
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
                ContactsContract.Contacts.InterfaceConsts.PhotoUri
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
                        Id = contactDetailCursor.GetInt(0),
                        DisplayName = contactDetailCursor.GetString(1)
                    };

                    string contactImagePath = contactDetailCursor.GetString(3);

                    if (contactImagePath != null)
                    {
                        try
                        {
                            await using var sourceStream = MauiApplication.Current.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(contactImagePath));
                            contact.Image = await ConvertToBase64Image(sourceStream);
                        }
                        catch (Java.IO.FileNotFoundException) { }
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

    async Task<string> ConvertToBase64Image(Stream stream)
    {
        byte[] bytes;

        await using (MemoryStream memoryStream = new())
        {
            await stream.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();
        }

        string base64 = Convert.ToBase64String(bytes);

        return $"data:image/jpg;base64, {base64}";
    }
}