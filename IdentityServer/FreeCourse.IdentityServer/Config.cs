using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using static IdentityServer4.Events.TokenIssuedSuccessEvent;

namespace FreeCourse.IdentityServer
{
    //Burası hangi identity serverden kimden token alacak, hangi microservislere istek yapılacağını belirleyecek
    //Reasurce identity de JWT token içinde = catolog için mesela mutlaka Aud:Reasurce_catalogda olması lazımm ve Scope:coursecatalog_fullpermission olması gerekiyor.istek yapıldığında JWT içinde bu bahsettiklerimi barındırır.diğerleride barındırır ama bunların iki özelliği olması gerekiyor.
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]//Aud lara karşılık geliyor
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},//catalog için full erişim.catalog_fullpermission=ApiScopes den bilgi alıyor.istek yapması gerekli
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},//photo için full erişim.ApiScopes den bilgi alıyor.istek yapması gerekli
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
            new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
            new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)//identity serverin kendine istek yapması için
        };
        //ApiResources i tanımlamalar yapıldıktan sonra, Startup a eklemeyi unutma.

        public static IEnumerable<IdentityResource> IdentityResources =>//get propertis
                   new IdentityResource[]
                   {//identiy ile gönderilecek bilgiler.yani login olduktan sonra dönüş yapılacak bilgiler.
                       new IdentityResources.Email(),//kullanıcının email erişimi için bilgi
                       new IdentityResources.OpenId(),//id mutlaka dolu olmak zorundadır.yoksa çalışmaz. email boş olabilir ancak openid mutlaka dolu olmalıdır.
                       new IdentityResources.Profile(),//profil ile ilgili bilgiler ilişkilendirilsin.
                       new IdentityResource(){ Name="roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new []{ "role"} }//gönderilirken kullanıcının name,role gönderilecek bilgiler.
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]//
            {
                new ApiScope("catalog_fullpermission","Catalog API için full erişim"),//catalog için full erişim için
                new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),//photo için full erişim
                new ApiScope("basket_fullpermission","Basket API için full erişim"),
                new ApiScope("discount_fullpermission","Discount API için full erişim"),
                new ApiScope("order_fullpermission","Order API için full erişim"),
                new ApiScope("payment_fullpermission","Payment API için full erişim"),
                new ApiScope("gateway_fullpermission","Gateway API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)//localapiden scopename bilgisini alıyor.identity serverin kendine istek yapması için
            };

        public static IEnumerable<Client> Clients =>//buradki Client identity servere göre ASP.NET CORE API dir.
            new Client[]//Client dediğimiz yer full Asp.Net MVC projesi. bu identityden farklı yerde o yüzden.birbirinden izole olduğu için MVC bir client dir.
            {
                //*****ClientCredentials da refresh token olmaz!!!!!!!!!!
                //Bu bilgiler memory de olacak


                //***Client identiyserver yapılandırılması
                new Client
                {
                   ClientName="Asp.Net Core MVC",//kim istiyor adı belli olsun.
                   ClientId="WebMvcClient",
                   ClientSecrets= {new Secret("secret".Sha256())},//şifre= secret olarak tanımladık.ama bunu şifrelemek lazım Sha256 şifreleme ile
                   AllowedGrantTypes= GrantTypes.ClientCredentials,//akış tipi ClientCredentials olarak belirlendi.ClientCredentials larda refresh token olmaz!!!
                   AllowedScopes={ "catalog_fullpermission","photo_stock_fullpermission", "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName }//kimlere izin verilecek.yeni kimlere token verilecek.
                },
                //***Resource identityserver yapılandırılması
                   new Client
                {
                   ClientName="Asp.Net Core MVC",
                   ClientId="WebMvcClientForUser",
                   AllowOfflineAccess=true,//refresh token izin veriyoruz
                   ClientSecrets= {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,//refresh token oluşturabiliriyoruz
                   AllowedScopes={ "basket_fullpermission", "order_fullpermission", "gateway_fullpermission", IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,"roles" },//token aldığında token ile birlikte bilgilerini eşibileceiği tüm bilgiler burada bulunuyor.
                   //yukarıda IdentityServerConstants.StandardScopes.OfflineAccess= refresh token oluşturma için kullanıyor. Yani bununla refresh token oluşturuluyor.
                   AccessTokenLifetime=1*60*60,// accessToken ömrünü saat bazında belirliyoruz.
                   RefreshTokenExpiration=TokenExpiration.Absolute,//eğer Absolute yerine Sliding seçerseniz refresh token bittikçe kendisi yeni refresh token üretir.
                   AbsoluteRefreshTokenLifetime= (int) (DateTime.Now.AddDays(60)- DateTime.Now).TotalSeconds,// refresh ömrünü 60 gün sonra bitecek. 
                   RefreshTokenUsage= TokenUsage.ReUse//refresh token ReUse yerine OneTimeOnly yaparsan bir kere kullanılır.
                //*******Kullanıcı siteye bir kere login olduğunda refresh token yenilenir. ve yukarıdaki kuralların hepsi yeniden başlar.
                   },
                      new Client
                {
                    ClientName="Token Exchange Client",
                    ClientId="TokenExhangeClient",
                    ClientSecrets= {new Secret("secret".Sha256())},
                    AllowedGrantTypes= new []{ "urn:ietf:params:oauth:grant-type:token-exchange" },
                    AllowedScopes={ "discount_fullpermission", "payment_fullpermission", IdentityServerConstants.StandardScopes.OpenId }
                },
            };
    }
}
