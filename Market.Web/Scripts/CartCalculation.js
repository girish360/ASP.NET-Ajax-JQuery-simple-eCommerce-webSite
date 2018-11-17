<script type="text/javascript">
    $(document).ready(function () {
            var countChecked = function () {
                var n = $("input:checked").length;
                let txt = (n + (n === 1 ? " is" : " are") + " checked!");
                let element = $(`<div>${txt}</div>`).css({'font-weight': 'bold' });

                $('.Pricecontainer').text(txt);


            };

            countChecked();




            $("input[type=checkbox]").on("click", countChecked);
        });
    </script>