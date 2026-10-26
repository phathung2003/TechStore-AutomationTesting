<?php

namespace App\Http\Controllers\Client;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Hash;
use App\Models\Rank;
use App\Models\Favorite;
use App\Models\Order;
use App\Models\OrderDetail;
use App\Models\Product;
use App\Models\Brand;
use App\Models\Category;
class DashBoardController extends Controller
{
    //
    public function home(){

        return view('client.home.index');
    }
    public function shop(){
        return view('client.shop.index');
    }
    public function blog(){
        return view('client.blog.index');
    }
    public function more(){

        return view('client.more.index');
    }
    

}
