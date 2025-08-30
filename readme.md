# Featured Clip Overlay for Streamer.bot

Fetch a random **featured** Twitch clip for a user, broadcast it, and autoplay it in an OBS browser overlay. Unloads after playback to free resources.

---

## Contents

* `StreamerBotAction.cs` – C# Code sub-action.
* `clip-overlay.html` – OBS overlay page.

---

## Requirements

* Streamer.bot with C# actions enabled.
* WebSocket Server enabled in Streamer.bot.
* HTTP Server enabled in Streamer bot
* OBS Studio with Browser Source.

---

## 1) Streamer.bot setup

### Enable WebSocket

`Server/Clients → WebSocket Server → Enable`.
Default host: `127.0.0.1`. Default port: `8080`. Add a password if desired.

### Enable HTTP Server

`Server/Clients → HTTP Server → Enable`.
Default host: `127.0.0.1`. Default port: `7474`.
Path Mapping: create PATH: `clip_overlay` to a new folder. 
Place `clip_overlay.html` in the new folder. 
(the html-file in the new folder is now accesible via `http://localhost:7474/clip_overlay/clip_overlay.html)

### Create C# Code sub-action

Create an Action → add **C# Code** sub-action → paste contents of `clip_overlay.cs`

**Triggering options**

* Manual: Run Action with set variable name `twitchUser` and value `twitchUsername`.

* Chat command example:

  * Add a **Command** like `!featuredclip {user}`.
  * set argument `{user}` to `twitchUser` argument in the action.

### Example Import code

see `streamerbot.export` - import the code to streamer.bot to get a working `!shoutout` command and a simple action. 

---

## 2) Use

* Add a browser source to OBS:
http://localhost:7474/clip-overlay.html?parent=localhost&host=127.0.0.1&port=8080

* In Streamer.bot, run the action
   The overlay autoplays a random featured clip and then unloads.

---

## Configuration

Query params on the overlay:

* `host` – Streamer.bot WS host. Default `localhost`.
* `port` – Streamer.bot WS port. Default `8080`.
* `password` – Streamer.bot WS password if set.
* `parent` – Required Twitch embed host. Must match page host.
* `muted` – `true` or `false`. Default `false`. Unmuted autoplay is not reliable.
* `fallbackSeconds` – Used when clip duration is missing. Default `30`.

Action args:

* `twitchUser` – Twitch login of the broadcaster to pull clips for. Required.
* `count` – Fetch pool size before random pick. Default `50`.

---

## Notes and limits

* Twitch **Clips** embed is iframe-only.
* `file://` cannot be used to provide the overlay. Twitch CPS `parent` requires a domain.
* Unmuted autoplay may be blocked by browser policies. Expect muted autoplay or manual unmute.
* The overlay removes the iframe after playback, its a simple timer, if loading takes too long or buffering occurs, the clips mights be cut short.

---

## Troubleshooting

**Blank player or “refused to connect”**

* `parent` mismatch. The value must equal the overlay page host.
* Using `file://`. Serve over HTTP.

**Overlay never receives events**

* Wrong WS host/port/password in URL.
* WebSocket Server disabled in Streamer.bot.
* Firewall blocks localhost port.

**Clip never unloads**

* Clip has no duration. Set `fallbackSeconds` or supply duration in payload.

---

## Security

* Keep the WebSocket on localhost.
* If you must expose it, set a strong WS password and limit network access.

---

## License

Use, modify, and ship as you see fit.