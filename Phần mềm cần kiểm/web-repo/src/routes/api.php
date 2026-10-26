<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\API\AuthController;
use App\Http\Controllers\API\DashboardController;
use App\Models\User;
use Illuminate\Support\Facades\Auth;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider and all of them will
| be assigned to the "api" middleware group. Make something great!
|
*/


Route::post('login', [AuthController::class,'login']);
Route::post('register', [AuthController::class,'register']);
Route::group(['middleware' => ['auth:sanctum'], 'as' => 'api.'], function () {
    Route::post('logout', [AuthController::class,'logout']);
    Route::get('user', [AuthController::class, 'getProfile']);
    Route::get('products',[DashboardController::class,'getListProduct']);
    Route::get('categories',[DashboardController::class,'getListCategory']);
    Route::get('brands',[DashboardController::class,'getListBrand']);
    Route::get('orders',[DashboardController::class,'getListOrder']);
    Route::post('orders',[DashboardController::class,'createOrder']);
    Route::get('orderdetails/{id}',[DashboardController::class,'getListOrderDetail']);
    Route::post('orderdetails',[DashboardController::class,'createOrderDetail']);

});




