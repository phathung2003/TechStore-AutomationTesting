<?php

namespace App\Http\Controllers\Client;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Order;
use App\Models\OrderDetail;
use App\Models\Product;

class UserOrderController extends Controller
{
    private $order;
    private $orderDetail;
    public function __construct(){
        $this->order = new Order();
        $this->orderDetail = new OrderDetail();
    }
    public function getOrderDetail($id) {
        $order = Order::where('user_id',$id)->get();
        if(!empty($order)){
            foreach($order as $item)
               {
                    $orderDetail = $this->orderDetail->where('order_id', $item->id)->get();
                    return view('client.profile.order',compact('orderDetail','order'));
                }
        }
        return view('client.profile.order');
    }
    public function cancelCart($id){
        $order = Order::where('id',$id)
                      ->update(['status' => 2]);

        return back();
    }
    public function deleteOrder($id=0){
        if(!empty($id) && ctype_digit($id)){
            $order = Order::find($id);
                if(!empty($order)) {
                    $orderDetail = $this->orderDetail->where('order_id', $id)->delete();
                    $check = $order->delete();
                    if($check) {
                        return back();
                    }
                }
        }
        return back();
    }
    public function getOrderConfirm($id){
        $order = Order::where('user_id',$id)->get();
        if(!empty($order)){
            return view('client.profile.confirmOrder',compact('order'));
        }
        return view('client.profile.confirmOrder');
    }
    public function getOrderShipped($id){
        $order = Order::where('user_id',$id)->where('status',3)->get();
        return view('client.profile.shippedOrder',compact('order'));
    }
    public function getOrderOntheway($id){
        $order = Order::where('user_id',$id)->where('status',4)->get();
        return view('client.profile.onthewayOrder',compact('order'));
    }
    public function getOrderDelivered($id){
        $order = Order::where('user_id',$id)->where('status',5)->get();
        return view('client.profile.deliveredOrder',compact('order'));
    }
    public function getOrdered($id){
        $order = Order::where('user_id',$id)->where('status',2)->get();
        return view('client.profile.Ordered',compact('order'));

    }
}
