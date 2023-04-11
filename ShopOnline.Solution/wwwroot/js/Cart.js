function MakeUpdateButtonVisible(id, visible) {
    var UpdateQtyButton = document.getElementById(id);
    if (visible == true) {
        UpdateQtyButton.style.display = "inline-block";
    }
    else {
        UpdateQtyButton.style.display = "none";
    }
}