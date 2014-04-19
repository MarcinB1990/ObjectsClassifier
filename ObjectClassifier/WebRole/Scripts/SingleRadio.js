function SelectOne(rdo, gridName) {
    all = document.getElementsByTagName("input");
    for (i = 2; i < all.length; i++) {
        if (all[i].type == "radio"){
                all[i].checked = false;
        }
    }
    rdo.checked = true;
}