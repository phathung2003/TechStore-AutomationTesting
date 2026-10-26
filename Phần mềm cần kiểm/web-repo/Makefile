build:
	- docker compose build
up-d:
	- docker compose up -d
stop: 
	- docker compose stop
composer-install:
	- docker compose run composer install
composer-autoload:
	- docker compose run composer dump-autoload
npm-install:
	- docker compose run npm install
npm-build:
	- docker compose run npm run build
key:
	- docker compose exec php php artisan key:generate
link:
	- docker compose exec php php artisan storage:link
migrate:
	- docker compose exec php php artisan migrate
migrate-reset:
	- docker compose exec php php artisan migrate:reset
model:
	- docker compose exec php php artisan make:model
view:
	- docker compose exec php php artisan make:view
controller:
	- docker compose exec php php artisan make:controller
config:
	- docker compose exec php php artisan config:cache
admin : 
	- docker compose exec php php artisan db:seed --class=AdminSeeder
