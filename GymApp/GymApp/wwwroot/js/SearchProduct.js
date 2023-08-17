function searchProducts() {
    var query = document.getElementById("searchQuery").value;
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "/Meal/Search?query=" + query, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var searchResultsDiv = document.getElementById("searchResults");
            searchResultsDiv.innerHTML = xhr.responseText;
        }
    };
    xhr.send();
}