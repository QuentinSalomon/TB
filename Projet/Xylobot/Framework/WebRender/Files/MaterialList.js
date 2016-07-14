<script src="jquery-1.12.2.min.js"></script>
<script src="woopsa-client.min.js"></script>
<script>
    var parent = document.getElementById("p");
    var myArray = [<%values%>];

    function AddItem(i){
            parent.appendChild(document.createElement("br"));

            <!--Ajoute la valeur à la liste-->
            var new_name = document.createElement("label");
            new_name.appendChild(document.createTextNode(myArray[i]));
            parent.appendChild(new_name);
        });
    }

    <!--Création de la liste-->
    for (var i = 0; i < <%count%>; i++){
        AddItem(i);
    }
</script>