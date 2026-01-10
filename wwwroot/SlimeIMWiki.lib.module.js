export async function afterStarted(blazor) {
    const config = blazor.runtime.getConfig();
    const exports = await blazor.runtime.getAssemblyExports(config.mainAssemblyName);
    
    exports.SlimeIMWiki.GlobalJsExport.SetIsOnline(navigator.onLine);

    addEventListener("online", _ => exports.SlimeIMWiki.GlobalJsExport.SetIsOnline(true));
    addEventListener("offline", _ => exports.SlimeIMWiki.GlobalJsExport.SetIsOnline(false));
}