<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Order;
use App\Models\OrderDetail;
class OrderController extends Controller
{
    //
    private $order;
    private $orderDetail;
    public function __construct(){
        $this->order = new Order();
        // $this->orderDetail = new OrderDetail();
    }
    public function getList(){
        $title = 'List of Orders';
        $list = Order::all();
        if(!empty($list)) {
            return view('admin.order.list', compact('title','list'));
        }
        return view('admin.order.list',compact('title'));
    }
    public function getOrderDetail($id) {
        $orderDetailList = OrderDetail::where('order_id', $id)->get();
        return view('admin.order.list_detail_order',compact('orderDetailList'));
    }
    public function getOrderHistory() {
        $order = $this->order->getList(auth()->user()->id);
        return view('client.order_history',compact('order'));
    }
    public function getUpdateStatusOrder($id){
        $order = $this->order->getListByIdOrder($id)[0];
        return view('admin.order.update',compact('order'));
    }
    public function postUpdateStatusOrder(Request $request, $id) {
        $this->order->updateStatusOrder($request->code,$request->status);
        return redirect()->route('admin.orders.getList')->with('msg','Cập nhật đơn hàng thành công');
    }
}
