# LemonsTiming24

Timing for a certain 24 hour car race

## Re-building templates

If you ever need to re-build the templates (such as upgrading between .net
previews), You can use `dotnet new blazorwasm --hosted true --auth None --name LemonsTiming24 --output .`, which will generate a new project.

Once the Blazor WASM templates have been updated, you can update the API components by using `dotnet new webapi --auth None --name LemonsTiming24.Server --output Server --force`.

## Dealing with UTF8 BOM and inconsistent line endings

### Fixing the problem once it's happened

Some of the .Net generated template files have a pesky UTF* BOM at the start of the file, and git sometimes strips CRLF. To fix these on Windows execute the following command (Reverse the order of the two `exec`s on unix):

*  ``find . -not -path './.git/*' -not -path './packages/*' -not -path './artifacts/*' -not -path './.vs/*' -not -type d -not \( -name '*.png' -o -name '*.eot' -o -name '*.otf' -o -name '*.ttf' -o -name '*.woff' \) -exec dos2unix {} \; -exec unix2dos {} \;``

### Prevent VS 2022 from creating BOM markers

You cna prevent VS2022 from creating the BOM by installing the [Force UTF-8 (No BOM) 2022](https://marketplace.visualstudio.com/items?itemName=qazwsxlty.forceutf8nobom2022) Visual Studio extension.

## Configuring for development

The SQL connection string and base URL are not kept in the source. Instead you will need to use [user secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) to manage them. Use ``dotnet user-secrets init  --project Server\`` to create the user secrets store for the application.

* To store the secret for the base URL, use ``dotnet user-secrets set "Timing:BaseUrl" "base URL" --project Server``