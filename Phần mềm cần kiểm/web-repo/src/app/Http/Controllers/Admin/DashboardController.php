<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Order;
use App\Models\OrderDetail;
use App\Models\Product;
use App\Models\Brand;
use App\Models\Category;
use Carbon\Carbon;
class DashboardController extends Controller
{
    //
    public function index(){
        $title = 'Dashboard';
        return view('admin.layouts.dashboard',compact('title'));
    }
    public function revenueSourcesCategory() {
        $list = Order::where('status','!=',1)->get();
        $res= ["Phone" => 0, "Laptop" => 0, "Watch" => 0, "Sound" => 0];
        foreach($list as $item) {
            $listDetail = OrderDetail::where('order_id', $item->id)->get();
            foreach($listDetail as $subItem) {
                $cate = getCategoryByProductId($subItem->product_id);
                $res[$cate]+=$subItem->amount;
            }
        }
        return $res;

    }
    public function revenueSourcesBrand() {
        $list = Order::where('status','!=',1)->get();
        $res= ["Apple" => 0, "SamSung" => 0, "Xiaomi" => 0, "Huawei" => 0];
        foreach($list as $item) {
            $listDetail = OrderDetail::where('order_id', $item->id)->get();
            foreach($listDetail as $subItem) {
                $cate = getBrandByProductId($subItem->product_id);
                $res[$cate]+=$subItem->amount;
            }
        }
        return $res;

    }
    public function earningsOverview(){
        $res = ["Jan" => 0, "Feb" => 0, "Mar" => 0, "Apr" => 0, "May" => 0, "Jun" => 0, "Jul" => 0, "Aug" => 0, "Sep" => 0, "Oct" => 0, "Nov" => 0, "Dec" => 0];
        $list = Order::where('status','!=',1)->get();
        foreach($list as $item) {
            $month = Carbon::parse($item->created_at)->format('m');
            $monthName = getNameMonth($month);
            $res[$monthName] += $item->total;
        }

        return $res;
    }
    public function numberOfProducts(){
        $res = ["Jan" => 0, "Feb" => 0, "Mar" => 0, "Apr" => 0, "May" => 0, "Jun" => 0, "Jul" => 0, "Aug" => 0, "Sep" => 0, "Oct" => 0, "Nov" => 0, "Dec" => 0];
        $list = Order::where('status','!=',1)->get();
        foreach($list as $item) {
            $listDetail = OrderDetail::where('order_id', $item->id)->get();
            foreach($listDetail as $subItem) {
                $month = Carbon::parse($subItem->created_at)->format('m');
                $monthName = getNameMonth($month);
                $res[$monthName] += $subItem->amount;
            }
        }
        return $res;
    }

}
