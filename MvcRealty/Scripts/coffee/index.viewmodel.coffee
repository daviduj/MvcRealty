define [
    'require',
    'jquery'
    'knockout',
    'ko-kendo'
], (require) ->
    ko = require 'knockout'
    $  = require 'jquery'

    class ViewModel
        #Model can be in contructor parameter
        constructor: () ->
            @date = ko.observable 'Feb-17-2016'
            @message = ko.observable 'Hey'
            @fullMessage = ko.computed =>
                @message() + " " + @date()
    
    ko.applyBindings(new ViewModel("CoffeeScript", "Fan"))
return