<?php

namespace App\Http\Controllers\API;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Auth;
use App\Models\Brand;
use App\Models\Category;
use App\Models\Product;
use App\Models\Order;
use App\Models\OrderDetail;

class DashboardController extends Controller
{
    public function getListProduct() {
        $list = Product::all();
        if(count($list) > 0) {
            return response($list,200);
        }
        return response("Not found",404);
    }
    public function getListCategory() {
        $list = Category::all();
        if(count($list) > 0) {
            return response($list,200);
        }
        return response("Not found",404);
    }
    public function getListBrand() {
        $list = Brand::all();
        if(count($list) > 0) {
            return response($list,200);
        }
        return response("Not found",404);
    }
    public function getListOrder(){
        $id = Auth::user()->id;
        $list = Order::where('user_id',$id)->get();
        if(count($list) > 0) {
            return response($list,200);
        }
        return response("Not found",404);
    }
    public function createOrder(Request $req){
        $order = new Order;
        $order->user_id = Auth::user()->id;
        $order->phone = Auth::user()->phone;
        $order->address = Auth::user()->address;
        $order->status = 1;
        $order->total = $req->total_price;
        $order->save();
        return response([
            "data" => $order
        ], 201);
    }
    public function createOrderDetail(Request $req){
        $order_detail = new OrderDetail;
        $order_detail->order_id = $req->order_id;
        $order_detail->product_id = $req->product_id;
        $order_detail->amount = $req->amount;
        $order_detail->price = $req->price;
        $order_detail->save();
        return response([
            "data" => $order_detail
        ], 201);
    }
    public function getListOrderDetail($id = 0) {
        $list = OrderDetail::where('order_id', $id)->get();
        if(count($list) > 0) {
            return response($list,200);
        }
        return response($id);
    }
}
