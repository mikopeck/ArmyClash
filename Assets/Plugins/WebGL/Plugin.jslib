mergeInto(LibraryManager.library, {

   //function to pass through text to inputData element in the html page
   PassText: function (text) {
      var convertedText = Pointer_stringify(text);
      var dataElement = document.getElementById("inputData");
      dataElement.value = convertedText;
   },
   //function to set fast data from the variable text
   SetFastData: function (text) {
      var data = Pointer_stringify(text);
      setEntry(data);
   },
   //function to get fast data from the variable text
   GetFastData: function (text) {
      var friendsKey = Pointer_stringify(text);
      getEntry(friendsKey);
   },
   //function to set slow data from the variable text
   SetSlowData: function (text) {
      var data = Pointer_stringify(text);
      setJSON(data);
   },
   //function to get slow data from the variable text
   GetSlowData: function (text) {
      var friendsKey = Pointer_stringify(text);
      getJSON(friendsKey);
   },
   InitMySky: function () {
      initMySky();
   },
   //function to get the keys needed(will be returned from the function)
   GetKeys: function () {
      getKeys();
   },
   // Play again
   ReloadGame: function() {
      reloadGame();
   },
   // add to content record
   AddContentRecord: function(text) {
      var data = Pointer_stringify(text);
      setContentRecord(data);
   }
});