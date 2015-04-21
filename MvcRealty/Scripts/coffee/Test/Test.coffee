define 'Test' , ['base'], (base) ->
  $container = $ ".tabs"
  base.activeTabs $container
  return