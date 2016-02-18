# CoffeeScript
require.config
    baseUrl: '/scripts/3rd',
    paths:
      'jquery'  : '/scripts/3rd/jquery-1.10.2',
      'knockout': '/scripts/3rd/knockout-3.3.0'
      'kendo'   : '/scripts/3rd/kendo.all',
      'ko-kendo': '/scripts/3rd/knockout-kendo.min'
    shim: 
        'jquery': 
            init: () ->
                    return window.jQuery;
            exports: '$'
         'kendo':
            deps: ['jquery'],
            init: ()->
                return window.kendo;
         'ko-kendo':
            deps: ['jquery', 'knockout', 'kendo']
            exports: 'ko-kendo'