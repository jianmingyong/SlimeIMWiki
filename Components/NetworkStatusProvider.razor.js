export function registerOnlineEvent(dotnet) {
    const listenerFunc = () => dotnet.invokeMethod('SetIsOnline', true);
    addEventListener("online", listenerFunc);
    return listenerFunc;
}

export function unregisterOnlineEvent(listener) {
    removeEventListener("online", listener);
}

export function registerOfflineEvent(dotnet) {
    const listenerFunc = () => dotnet.invokeMethod('SetIsOnline', false);
    addEventListener("offline", listenerFunc);
    return listenerFunc;
}

export function unregisterOfflineEvent(listener) {
    removeEventListener("offline", listener);
}