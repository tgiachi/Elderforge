log_info("Bootstraping...")

set_game_cfg({})

add_admin("admin", "password")

on_bootstrap(function()
    log_info("Bootstraping done.")
end)
