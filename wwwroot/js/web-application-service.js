export function isOnline() {
    return navigator.onLine;
}

export function registerEventListener(dotnet) {
    addEventListener("online", _ => dotnet.invokeMethod("SetIsOnline", true));
    addEventListener("offline", _ => dotnet.invokeMethod("SetIsOnline", false));
}