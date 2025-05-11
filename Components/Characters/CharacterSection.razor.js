export function getScrollPosition(elementId) {
    return $(`#${elementId}`).scrollTop();
}

export function getElementWidth(elementId) {
    return $(`#${elementId}`).width();
}

export function getElementHeight(elementId) {
    return $(`#${elementId}`).height();
}