$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var query = $(this).val();

        if (query.length >= 3) {
            $.ajax({
                url: '/Meal/SearchBar?query=' + query,
                data: { query: query },
                dataType: 'json',
                success: function (data) {
                    var resultsDiv = $('#searchResults');
                    resultsDiv.empty();

                    if (data.length > 0) {
                        var ul = $('<ul>');
                        $.each(data, function (index, item) {
                            var productName = item.productName;
                            var productId = item.productId;
                            var listItem = $('<li>').html('<a href="#" data-productid="' + productId + '">' + productName + '</a>');
                            ul.append(listItem);
                        });
                        resultsDiv.append(ul);
                    } else {
                        resultsDiv.text('No results found.');
                    }
                },
            });
        } else {
            $('#searchResults').empty();
        }
    });
});