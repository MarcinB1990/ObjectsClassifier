function SelectOne(rdo, gridName) {
    all = document.getElementsByTagName("input");
    for (i = 0; i < all.length; i++) {
        if (all[i].type == "radio" && all[i].id != 'MainContent_radioNewOrOldTrainingSet_0' && all[i].id != 'MainContent_radioNewOrOldTrainingSet_1') {
                all[i].checked = false;
        }
    }
    rdo.checked = true;
}