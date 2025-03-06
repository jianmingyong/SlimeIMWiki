function updateUnitIcons() {
    let unitIcons = document.getElementsByClassName("unitIcon");

    for (const unitIconsKey of unitIcons) {
        let fn = function () {
            let width = unitIconsKey.naturalWidth;
            let height = unitIconsKey.naturalHeight;

            if (width === height) {
                unitIconsKey.classList.add("w-100");
                unitIconsKey.classList.add("h-100");
                unitIconsKey.classList.add("start-50")
                unitIconsKey.classList.add("translate-middle-x")
            } else {
                unitIconsKey.classList.add("bottom-0");
                unitIconsKey.classList.add("w-auto");
                unitIconsKey.classList.add("h-100");

                let currentRect = unitIconsKey.getBoundingClientRect();

                if (currentRect.width > 90)
                {
                    unitIconsKey.style.setProperty("transform", "translateX(-" + (currentRect.width - currentRect.height) / 2 + "px)", "important");
                }
                else
                {
                    if (unitIconsKey.classList.contains("facingLeft"))
                    {
                        unitIconsKey.style.setProperty("transform", "translateX(" + (currentRect.height - currentRect.width) + "px)", "important");
                    }
                }
            }
        };

        unitIconsKey.onload = fn;

        if (unitIconsKey.complete) {
            fn();
        }
    }
}