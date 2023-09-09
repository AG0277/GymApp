document.addEventListener("DOMContentLoaded", function () {
    var mealName = document.getElementById("mealName");
    var headerText = document.getElementById("headerText");

    // Add an event listener to the input element
    mealName.addEventListener("input", function () {
        headerText.innerText = mealName.value;
    });
});