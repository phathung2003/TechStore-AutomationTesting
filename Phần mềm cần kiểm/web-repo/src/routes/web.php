<?php

use App\Http\Controllers\Admin\DashboardController as AdminDashboardController;
use App\Http\Controllers\Admin\ProductController as AdminProductController;
use App\Http\Controllers\Admin\CategoryController as AdminCategoryController;
use App\Http\Controllers\Admin\BrandController as AdminBrandController;
use App\Http\Controllers\Admin\OrderController as AdminOrderController;
use App\Http\Controllers\Client\DashboardController as ClientDashboardController;
use App\Http\Controllers\Client\ProductController as ClientProductController;
use App\Http\Controllers\MyAuth\AuthController;
use App\Http\Controllers\Admin\DiscountController;
use App\Http\Controllers\Admin\RankController;
use App\Http\Controllers\Client\UserController;
use App\Http\Controllers\Client\FavoriteController;
use App\Http\Controllers\Client\UserOrderController;
use App\Http\Controllers\Client\CommentController;
use App\Http\Controllers\Client\CartController;
use Illuminate\Support\Facades\Route;
use Illuminate\Support\Facades\Auth;
use App\Models\User;
/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider and all of them will
| be assigned to the "web" middleware group. Make something great!
|
*/

// Routes Admin
Route::prefix('admin')->name('admin.')->middleware('admin')->group(function (){
    Route::post('logout', [AuthController::class,'logout'])->name('logout');
    Route::get('/',[AdminDashboardController::class, 'index'])->name('index');
    Route::get('/revenueSourcesCategory',[AdminDashboardController::class, 'revenueSourcesCategory'])->name('revenueSourcesCategory');
    Route::get('/revenueSourcesBrand',[AdminDashboardController::class, 'revenueSourcesBrand'])->name('revenueSourcesBrand');
    Route::get('/earningsOverview',[AdminDashboardController::class, 'earningsOverview'])->name('earningsOverview');
    Route::get('/numberOfProducts',[AdminDashboardController::class, 'numberOfProducts'])->name('numberOfProducts');

    Route::prefix('products')->name('products.')->group(function () {
        Route::get('/', [AdminProductController::class, 'getList'])->name('getList');
        Route::get('/add', [AdminProductController::class, 'getAdd'])->name('getAdd');
        Route::post('/add', [AdminProductController::class, 'postAdd'])->name('postAdd');
        Route::get('/update/{id}', [AdminProductController::class, 'getUpdate'])->name('getUpdate');
        Route::post('/update/{id}', [AdminProductController::class, 'postUpdate'])->name('postUpdate');
        Route::get('deleteItem/{id}', [ AdminProductController::class, 'deleteItem'])->name('delete');
    });
    Route::prefix('categories')->name('categories.')->group(function () {
        Route::get('/', [AdminCategoryController::class, 'getList'])->name('getList');
        Route::get('/add', [AdminCategoryController::class, 'getAdd'])->name('getAdd');
        Route::post('/add', [AdminCategoryController::class, 'postAdd'])->name('postAdd');
        Route::get('/update/{id}', [AdminCategoryController::class, 'getUpdate'])->name('getUpdate');
        Route::post('/update/{id}', [AdminCategoryController::class, 'postUpdate'])->name('postUpdate');
        Route::get('deleteItem/{id}', [ AdminCategoryController::class, 'deleteItem'])->name('delete');
    });
    Route::prefix('brands')->name('brands.')->group(function () {
        Route::get('/', [AdminBrandController::class, 'getList'])->name('getList');
        Route::get('/add', [AdminBrandController::class, 'getAdd'])->name('getAdd');
        Route::post('/add', [AdminBrandController::class, 'postAdd'])->name('postAdd');
        Route::get('/update/{id}', [AdminBrandController::class, 'getUpdate'])->name('getUpdate');
        Route::post('/update/{id}', [AdminBrandController::class, 'postUpdate'])->name('postUpdate');
        Route::get('deleteItem/{id}', [AdminBrandController::class, 'deleteItem'])->name('delete');
    });
    Route::prefix('discounts')->name('discount.')->group(function () {
        Route::get('/', [DiscountController::class, 'getList'])->name('getList');
        Route::get('/add', [DiscountController::class, 'getAdd'])->name('getAdd');
        Route::post('/add', [DiscountController::class, 'postAdd'])->name('postAdd');
        Route::get('/update/{id}', [DiscountController::class, 'getUpdate'])->name('getUpdate');
        Route::post('/update/{id}', [DiscountController::class, 'postUpdate'])->name('postUpdate');
        Route::get('deleteItem/{id}', [DiscountController::class, 'deleteItem'])->name('delete');
        Route::get('/search-voucher', [DiscountController::class, 'search'])->name('search-voucher');
    });
    Route::prefix('ranks')->name('rank.')->group(function () {
        Route::get('/', [RankController::class, 'getList'])->name('getList');
        Route::get('/add', [RankController::class, 'getAdd'])->name('getAdd');
        Route::post('/add', [RankController::class, 'postAdd'])->name('postAdd');
        Route::get('/update/{id}', [RankController::class, 'getUpdate'])->name('getUpdate');
        Route::post('/update/{id}', [RankController::class, 'postUpdate'])->name('postUpdate');
        Route::get('deleteItem/{id}', [RankController::class, 'deleteItem'])->name('delete');
    });
    Route::prefix('orders')->name('orders.')->group(function () {
        Route::get('/', [AdminOrderController::class, 'getList'])->name('getList');
        // Route::get('/detail/{id}', [AdminOrderController::class, 'getOrderDetail'])->name('getDetail');
        Route::get('/getUpdateStatusOrder/{id}', [AdminOrderController::class, 'getUpdateStatusOrder'])->name('getUpdateStatusOrder');
        Route::post('/getUpdateStatusOrder/{id}', [AdminOrderController::class, 'postUpdateStatusOrder'])->name('postUpdateStatusOrder');
        Route::get('/detailorder/{id}', [AdminOrderController::class, 'getOrderDetail'])->name('detailOrder');
    });

});

// Routes Client
Route::get('/', [ClientDashboardController::class, 'home'])->name('home');
Route::get('/shop', [ClientProductController::class, 'getList'])->name('shop');
Route::get('/getCategory/{data}', [ClientProductController::class, 'searchCategory'])->name('getCategory');
Route::get('/search', [ClientProductController::class, 'search'])->name('search');
Route::get('/shop/detailProduct/{id}', [ClientProductController::class, 'getDetailProduct'])->name('getDetailProduct');
Route::get('/blog', [ClientDashboardController::class, 'blog'])->name('blog');
Route::get('/more', [ClientDashboardController::class, 'more'])->name('more');
Route::get('addToCart/{id}',[CartController::class,'addToCart'])->name('addToCart');
Route::get('/shop/detailProduct/addCart/{id}', [CartController::class, 'addToCart'])->name('getDetailProductAddtoCart');
Route::middleware('auth')->group(function () {
    Route::get('cart', [CartController::class,'showCart'])->name('cart');
    Route::get('cart/checkout',[CartController::class,'checkout'])->name('checkOut');
    Route::get('/cancelCart/{id}', [UserOrderController::class, 'cancelCart'])->name('cancelCart');
    Route::post('cart/payment',[CartController::class,'checkPayment'])->name('checkPayment');
    Route::post('checkDiscount',[CartController::class,'checkDiscount'])->name('checkDiscount');
    Route::get('/cart/change-info', [CartController::class,'getChangeInfoOrder'])->name('getChangeInfoOrder');
    Route::post('/cart/change-info', [CartController::class,'postChangeInfoOrder'])->name('postChangeInfoOrder');
    Route::get('/getUserOrder/{id}', [UserOrderController::class, 'getOrderDetail'])->name('getAllUserOrder');
    Route::get('/getOrderConfirm/{id}', [UserOrderController::class, 'getOrderConfirm'])->name('getOrderConfirm');
    Route::get('/getOrderShipped/{id}', [UserOrderController::class, 'getOrderShipped'])->name('getOrderShipped');
    Route::get('/getOrderOntheway/{id}', [UserOrderController::class, 'getOrderOntheway'])->name('getOrderOntheway');
    Route::get('/getOrderDelivered/{id}', [UserOrderController::class, 'getOrderDelivered'])->name('getOrderDelivered');
    Route::get('/getOrdered/{id}', [UserOrderController::class, 'getOrdered'])->name('getOrdered');
    Route::get('updateItemListCart/{id}/{quantity}',[CartController::class,'updateItemListCart'])->name('updateItemListCart');
    Route::get('deleteItemListCart/{id}',[CartController::class,'deleteItemListCart'])->name('deleteItemListCart');
    Route::get('deleteOrder/{id}',[UserOrderController::class,'deleteOrder'])->name('deleteOrder');
    Route::get('/userProfile', [UserController::class, 'getUserProfile'])->name('getUserProfile');
    Route::post('/userProfile' , [UserController::class, 'postUserProfile'])->name('postUserProfile');
    Route::get('/userOrder' , [UserController::class, 'getUserOrder'])->name('getUserOrder');
    Route::get('/changePassword' , [UserController::class, 'getChangePassword'])->name('changePassword');
    Route::post('/changePassword' , [UserController::class, 'postChangePassword'])->name('postChangePassword');
    Route::post('checkDiscountSelect',[CartController::class,'checkDiscountSelect'])->name('checkDiscountSelect');
    Route::post('/product/comment/{id}', [CommentController::class, 'getComment'])->name('getComment');
    Route::get('/shop/favorite', [FavoriteController::class, 'getList'])->name('getFavorite');
    Route::get('shop/detailProduct/postFavorite/{id}', [FavoriteController::class, 'postAdd'])->name('postFavorite');
    Route::get('shop/deleteFavorite/{id}', [FavoriteController::class, 'deleteItem'])->name('deleteFavorite');
    Route::post('logout', [AuthController::class,'logout'])->name('logout');
});

Route::post('login', [AuthController::class,'login'])->name('login');
Route::post('register', [AuthController::class,'register'])->name('register');
// Auth::routes();
Route::get('home', function(){
    return view('layouts.app');
});



