define(['require'
       , 'jquery'
       , 'knockout'
       , 'ko-kendo'
], function (require) {
   var $, ko;
   ko = require('knockout');
   $ = require('jquery');

   function ViewModel() {
     var self = this;
     self.date = ko.observable('Feb-17-2016');
     self.message = ko.observable('Hey');
     self.fullMessage = ko.computed(function () {
       return self.message() + " " + self.date();
     });
   }
   ko.applyBindings(new ViewModel("CoffeeScript", "Fan"));
});