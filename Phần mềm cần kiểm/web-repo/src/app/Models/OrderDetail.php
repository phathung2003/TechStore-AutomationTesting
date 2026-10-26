<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use DB;

class OrderDetail extends Model
{
    use HasFactory;
    protected $table = 'order_details';
    public function getList($id=null){
        if(!empty($id)){
            $list = OrderDetail::where('order_id',$id)->orderBy('created_at','DESC')->get();
            return $list;
        }else{
            $list = OrderDetail::orderBy('created_at','DESC')->get();
            return $list;
        }
    }
    public function addOrderDetail($data){
        $order = DB::table($this->table)->insert($data);
        //$order = order_details::insert($data);
        return $order;
    }
    public function getTotal($id)
    {
        if(!empty($id)){
            $list = DB::table($this->table)->where('order_id',$id)->select('price')->get();
            return $list;
        }
            return 0;
    }
}
