# CoffeeScript
define 'base', [], () ->
  activeTabs = ($container) ->
    $container.on 'click', '.tab-bar a', (evt)->
      evt.preventDefault()
      $(@).parents(".tab-bar").find("a").each () -> 
        $(@).removeClass("selected")
        $($(@).data("view")).hide() if $(@).data("view")
      $(@).addClass("selected")
      $($(@).data("view")).show() if $(@).data("view")
  exports = 
    activeTabs: activeTabs