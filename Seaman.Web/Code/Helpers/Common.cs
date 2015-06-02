using System;

namespace Seaman.Web.Code.Helpers
{
    public static class Common
    {
        
        public static String DefualtUserFirstNameClaimType =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/firstname";

        public static String DefualtUserLastNameClaimType =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/lastname";

        public static String DefualtUserFullNameClaimType =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/fullname";

        public static String DefualtUserGenderClaimType =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";

        public static String DefualtUserPictureUrlClaimType =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/pictureurl";

        public static String DefualtUserThumbUrlClaimType =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumburl";

        public static String ImageThumbVariant = "thumb";
        public static String ImageProfilePictureVariant = "profilepicture";
        public static String PdfVariant = "pdf";
    }
}