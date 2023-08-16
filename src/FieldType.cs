namespace LunAdd
{
    public enum FieldType
    {
        FullName,
        FirstName,
        LastName,
        DisplayName,
        //
        HomeStreet,
        HomeCity,
        HomeZipCode,
        HomeCountry,
        Company,
        PrimaryEmail,
        //
        WorkAddress,
        WorkZipCode,
        WorkCity,
        WorkCountry,
        BusinessEMail,
        //
        WorkPhone,
        HomePhone,
        FaxNumber,
        CellularNumber,
        OtherPhone,
        //
        Notes,
        WebPage1,
        WebPage2,
        AdressList,
        PrimaryBusinessEMail,
        SecondEmail,
        NameList,
    }

    public static class VCardFields
    {
        //N:Surname;Christian;some;extra;infomaybe
        //FN:Full Name String Displayed
        //TEL;TYPE=work;VALUE=TEXT:033...
        //TEL;TYPE=fax;VALUE=TEXT:033...
        //TEL;TYPE=cell;VALUE=TEXT:0177-..
        //ADR:;;;Berlin;;;
        //EMAIL;PREF=1:this.just.test@testplace.nope
        //NOTE:Schuhfabrik:Wir machen das ...\nMore to say...
    }
}
